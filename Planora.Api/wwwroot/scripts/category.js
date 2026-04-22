const API = "https://localhost:7127/api/Category"
// --- Category management ---
const nameInput = document.getElementById('category-name')
const contentInput = document.getElementById('category-content')
const colorInput = document.getElementById('category-color')
const addBtn = document.getElementById('add-category-btn')
const listEl = document.getElementById('category-list')
const emptyState = document.getElementById('category-empty')

addBtn.addEventListener('click', async () => await createCategory())
nameInput.addEventListener('keydown', async e => {
    if (e.key === 'Enter') {
        await createCategory()
    }
})
contentInput.addEventListener('keydown', async e => {
    if (e.key === 'Enter') {
        await createCategory()
    }
})

MicroModal.init({
    disableScroll: true,          // lås baggrunds-scroll mens modal er åben
    awaitCloseAnimation: true,    // vent på CSS-animationen før DOM opdateres
})

let pendingDeleteId = null
const confirmTitle = document.getElementById('modal-action-title')
const confirmMessage = document.getElementById('modal-action-content')
const confirmBtn = document.getElementById('confirm-action-btn')

confirmBtn.addEventListener('click', async () => {
    MicroModal.close('modal-action')
    await deleteCategory(pendingDeleteId)
})

render()

async function render() {
    const categories = await getAllCategories()
    listEl.querySelectorAll('.category-card').forEach(el => el.remove())

    if (categories.length === 0) {
        emptyState.style.display = ''
        return
    }

    emptyState.style.display = 'none'

    categories.forEach((cat, i) => {
        const card = document.createElement('div')
        card.id = cat.categoryId
        card.className = 'category-card'
        card.style.animationDelay = `${i * 40}ms`

        // Color picker
        const colorEdit = document.createElement('input')
        colorEdit.type = 'color'
        colorEdit.className = 'category-color-edit'
        colorEdit.value = cat.hexColor
        colorEdit.style.background = cat.hexColor
        colorEdit.disabled = true

        // Text wrapper with name + content
        const textWrap = document.createElement('div')
        textWrap.className = 'category-card-text'

        const nameEdit = document.createElement('input')
        nameEdit.type = 'text'
        nameEdit.className = 'category-name-edit'
        nameEdit.value = cat.name
        nameEdit.maxLength = 40
        nameEdit.disabled = true

        const contentEdit = document.createElement('input')
        contentEdit.type = 'text'
        contentEdit.className = 'category-content-edit'
        contentEdit.value = cat.content
        contentEdit.maxLength = 200
        contentEdit.placeholder = 'No content'
        contentEdit.disabled = true

        textWrap.append(nameEdit, contentEdit)

        // Action buttons
        const actions = document.createElement('div')
        actions.className = 'category-card-actions'

        const editBtn = makeIconButton('category-edit-btn', 'edit', 'Edit category')
        const saveBtn = makeIconButton('category-save-btn', 'check', 'Save changes', true)
        const cancelBtn = makeIconButton('category-cancel-btn', 'close', 'Cancel edit', true)
        const deleteBtn = makeIconButton('category-delete-btn', 'delete', 'Delete category')

        actions.append(editBtn, saveBtn, cancelBtn, deleteBtn)
        card.append(colorEdit, textWrap, actions)

        const enterEditMode = () => {
            card.classList.add('editing')
            nameEdit.disabled = false
            contentEdit.disabled = false
            colorEdit.disabled = false
            editBtn.style.display = 'none'
            deleteBtn.style.display = 'none'
            saveBtn.style.display = ''
            cancelBtn.style.display = ''
            nameEdit.focus()
            nameEdit.select()
        }

        const exitEditMode = () => {
            card.classList.remove('editing')
            nameEdit.disabled = true
            contentEdit.disabled = true
            colorEdit.disabled = true
            editBtn.style.display = ''
            deleteBtn.style.display = ''
            saveBtn.style.display = 'none'
            cancelBtn.style.display = 'none'
        }

        const save = async () => {
            const newName = nameEdit.value.trim()
            const newContent = contentEdit.value.trim()
            const newColor = colorEdit.value

            if (!newName || !newContent) {
                const target = !newName ? nameEdit : contentEdit
                target.focus()
                target.classList.add('shake')
                setTimeout(() => target.classList.remove('shake'), 400)
                return
            }

            if (newName === cat.name && newContent === cat.content && newColor === cat.hexColor) {
                exitEditMode()
                return
            }

            await updateCategory(cat.categoryId, {
                name: newName,
                content: newContent,
                hexColor: newColor
            })
            await render()
        }

        editBtn.addEventListener('click', enterEditMode)
        cancelBtn.addEventListener('click', () => {
            nameEdit.value = cat.name
            contentEdit.value = cat.content
            colorEdit.value = cat.hexColor
            exitEditMode()
        })
        saveBtn.addEventListener('click', save)

        // Micromodal
        deleteBtn.addEventListener('click', async () => {
            confirmTitle.textContent = `Delete category`
            confirmMessage.textContent = `This will permanently delete the category "${cat.name}". This cannot be undone.`
            confirmBtn.textContent = `Delete`
            pendingDeleteId = cat.categoryId
            MicroModal.show('modal-action')
        })
        
        listEl.appendChild(card)
    })
}

function makeIconButton(className, iconName, label, hidden = false) {
    const btn = document.createElement('button')
    btn.className = className
    btn.type = 'button'
    btn.setAttribute('aria-label', label)
    btn.title = label
    if (hidden) btn.style.display = 'none'

    const icon = document.createElement('span')
    icon.className = 'material-symbols-outlined'
    icon.textContent = iconName
    btn.appendChild(icon)

    return btn
}

async function createCategory() {
    const name = nameInput.value.trim()
    const content = contentInput.value.trim()
    const hexColor = colorInput.value

    if (!name || !content) {
        const target = !name ? nameInput : contentInput
        target.focus()
        target.classList.add('shake')
        setTimeout(() => target.classList.remove('shake'), 400)
        return
    }

    // Matches CategoryDTO(string? CategoryId, string Name, string Content, string HexColor)
    // CategoryId is not included — generated server-side
    const category = { name, content, hexColor }

    await apiFetch(API, {
        method: "POST",
        body: JSON.stringify(category)
    })
    await render()

    nameInput.value = ''
    contentInput.value = ''
    nameInput.focus()
}

async function getAllCategories() {
    return await apiFetch(API)
}

async function updateCategory(id, category) {
    await apiFetch(API + `/${id}`, {
        method: "PUT",
        body: JSON.stringify(category)
    })
}

async function deleteCategory(id) {
    await apiFetch(API + `/${id}`, {
        method: "DELETE"
    })
    await render()
}

async function apiFetch(url, options = {}) {
    const token = sessionStorage.getItem("token")

    const response = await fetch(url, {
        ...options,
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`,
            ...options.headers
        }
    })

    if (response.status === 401) {
        sessionStorage.removeItem("token")
        window.location.href = "/"
        return
    }

    if (response.status === 403) {
        alert("You do not have permission to do this");
        return
    }

    if (response.status === 204) {
        return null
    }

    return response.json()
}