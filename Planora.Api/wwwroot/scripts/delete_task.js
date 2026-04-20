
document.getElementById("delete_task_btn").addEventListener("click", delete_task_handler)

async function delete_task_handler() {
    try {
        const id = get_task_id()
        if (!id) {
            return
        }
        await delete_task(url, error_message, id)
        reset_top_bar()
        
    } catch (error) {
        error_message(error.message)
    }
}

function get_task_id() {
	return document.querySelector(".topbar").id
}

async function delete_task(url, error_message, id) {
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
            throw new Error(error_message)
        }
    } catch (error) {
        throw new Error("unexcpeted issue with deletion")
    }
}

function error_message(message) {
    alert(message)
}
