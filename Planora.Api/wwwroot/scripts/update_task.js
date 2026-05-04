document.getElementById("update_task_btn").addEventListener("click", update_task_handler)

async function update_task_handler() {
    try {
        const task = get_task()
        if (!task.taskId) {
            return
        }
        await put(`/api/task/${task.taskId}`, task)
        reset_top_bar()
    } catch (error) {
        error_message(error.message)
    }
}

function get_task() {
    const id = document.querySelector(".topbar").id.trim()
    const title = document.querySelector(".task-name").value.trim()
    const content = document.querySelector(".task-content").value.trim()
    const categoryId = document.querySelector(".task-category").value.trim()
    const date = document.querySelector(".task-date").value
    return {
        taskId: id,
        content: content,
        title: title,
        categoryId: categoryId,
        deadline: date
    }
}