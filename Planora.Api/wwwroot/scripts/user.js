const API = "https://localhost:7127/api/User"
// --- Category management ---
const emailInput = document.getElementById('user-email-input')
const sendInvitationBtn = document.getElementById('user-send-invitation-btn')
const listEl = document.getElementById('user-list')
const emptyState = document.getElementById('user-empty')

sendInvitationBtn.addEventListener('click', async () => await sendInvitation())
emailInput.addEventListener('keydown', async e => {
    if (e.key === 'Enter') {
        await sendInvitation()
    }
})

render()

async function render() {
    const users = await getAllUsers()
    listEl.querySelectorAll('.user-card').forEach(el => el.remove())

    if (users.length === 0) {
        emptyState.style.display = ''
        return
    }

    emptyState.style.display = 'none'

    users.forEach((user, i) => {
        const card = document.createElement('div')
        card.id = user.userId
        card.className = 'user-card'
        card.style.animationDelay = `${i * 40}ms`

        // Text wrapper with full name and role
        const textWrap = document.createElement('div')
        textWrap.className = 'user-card-text'

        const fullNameDisplay = document.createElement('p')
        fullNameDisplay.className = 'user-full-name-display'
        fullNameDisplay.textContent = user.firstName + '' + user.lastName 

        const emailDisplay = document.createElement('p')
        emailDisplay.className = 'user-email-display'
        emailDisplay.textContent = user.email

        const roleWrap = document.createElement('label')
        roleWrap.className = 'user-role-edit'

        const roleEdit = document.createElement('input')
        roleEdit.type = 'checkbox'
        roleEdit.checked = user.tovholder
        roleEdit.disabled = true

        roleWrap.append(roleEdit, document.createTextNode(' Coordinator'))

        textWrap.append(fullNameEdit, emailEdit, roleWrap)

        // Action buttons
        const actions = document.createElement('div')
        actions.className = 'user-card-actions'

        const editBtn = makeIconButton('user-edit-btn', 'edit', 'Edit user')
        const saveBtn = makeIconButton('user-save-btn', 'check', 'Save changes', true)
        const cancelBtn = makeIconButton('user-cancel-btn', 'close', 'Cancel edit', true)
        const deleteBtn = makeIconButton('user-delete-btn', 'delete', 'Delete user')

        actions.append(editBtn, saveBtn, cancelBtn, deleteBtn)
        card.append(colorEdit, textWrap, actions)

        const enterEditMode = () => {
            card.classList.add('editing')
            roleEdit.disabled = false
            editBtn.style.display = 'none'
            deleteBtn.style.display = 'none'
            saveBtn.style.display = ''
            cancelBtn.style.display = ''
            roleEdit.focus()
        }

        const exitEditMode = () => {
            card.classList.remove('editing')
            roleEdit.disabled = true
            editBtn.style.display = ''
            deleteBtn.style.display = ''
            saveBtn.style.display = 'none'
            cancelBtn.style.display = 'none'
        }

        const save = async () => {
            const newRole = roleEdit.checked
            if (newRole === user.tovholder) {
                exitEditMode()
                return
            }

            await updateUser(user.userId, {
                firstName: user.firstName,
                lastName: user.lastName,
                tovholder: newRole
            })
            await render()
        }

        editBtn.addEventListener('click', enterEditMode)
        cancelBtn.addEventListener('click', () => {
            roleEdit.checked = user.tovholder
            exitEditMode()
        })
        saveBtn.addEventListener('click', save)

        deleteBtn.addEventListener('click', async () => await deleteUser(user.userId))
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

async function sendInvitation() {
    const email = emailInput.value.trim()

    if (!email) {
        emailInput.focus()
        emailInput.classList.add('shake')
        setTimeout(() => emailInput.classList.remove('shake'), 400)
        return
    }

    emailInput.value = ''
    emailInput.focus()
}

async function getAllUsers() {
    return await apiFetch(API)
}

async function updateUser(id, user) {
    await apiFetch(API + `/${id}`, {
        method: "PUT",
        body: JSON.stringify(user)
    })
}

async function deleteUser(id) {
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