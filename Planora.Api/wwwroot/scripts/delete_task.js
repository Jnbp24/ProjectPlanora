document.getElementById("delete_task_btn").addEventListener("click", async () =>{
    create = false
    const title = document.querySelector(".task-name").value;
    confirmTitle.textContent = `Delete task`
    confirmMessage.textContent = `This will permanently delete the task "${title}". This cannot be undone.`
    confirmBtn.textContent = `Delete`
    MicroModal.show('modal-action')
})

async function delete_task_handler() {
    try {
        const id = get_task_id()
        if (!id) {
            return
        }
        await delete_task(id)
        reset_top_bar()
    } catch (error) {
        error_message(error.message)
    }
}

function get_task_id() {
	return document.querySelector(".topbar").id
}

async function delete_task(id) {
    const token = sessionStorage.getItem("token")

    try {
        const response = await fetch(`api/task/${id}`, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            }
        })
        if (!response.ok) {
            throw new Error("couldn't delete task")
        }
    } catch (error) {
        throw new Error("unexcpeted issue with deletion")
    }
}

function error_message(message) {
    alert(message)
}
