const url = "api/task";
const read_fail = "Failed to read tasks";

let calendar;

async function setup_calendar() {
    try {
        const data = await get(url, read_fail);
        const events = map_to_event(data)

        create_calendar(events)
    } catch (error) {
        error_message(error.message);
    }
}

async function refresh_calendar() {
    try {
        const data = await get(url, read_fail);

        if (!calendar) return;

        calendar.removeAllEvents();
        calendar.addEventSource(map_to_event(data));

    } catch (error) {
        error_message(error.message);
    }
}

function map_to_event(data) {
    return data.map(task => ({
        id: task.taskId,
        title: task.title,
        start: task.deadline,
        extendedProps: {
            content: task.content,
            category: task.category
        },
        allDay: true
    }));
}

function create_calendar(tasks) {
    const calendarElement = document.getElementById("calendar");

    if (!calendarElement) return;

    calendar = new FullCalendar.Calendar(calendarElement, {
        initialView: 'listMonth',
        events: tasks,
        eventClick: task_click_handler,
        eventContent: function (arg) {
            return {
                html: `
                    <span class="fc-event-content">${arg.event.extendedProps.content ?? ''}</span>
                `
            }
        }
    })

    calendar.render();
}

function task_click_handler(info) {
    const data = info.event
    const task = map_to_task(data)

    const topbar_element = document.querySelector(".topbar")
    if (topbar_element.id === task.id) {
        reset_top_bar()
        return
    }
    topbar_element.id = task.id

    const delete_task_btn = document.getElementById("delete_task_btn")
    delete_task_btn.classList.remove("invisible")

    const update_task_btn = document.getElementById("update_task_btn")
    update_task_btn.classList.remove("invisible")

    const newTaskBtn = document.querySelector(".new-task-btn")
    newTaskBtn.classList.add("btn-disabled")
    newTaskBtn.disabled = true
    
    const titleInput = document.querySelector(".task-name")
    const contentInput = document.querySelector(".task-content")
    const categoryInput = document.querySelector(".task-category")
    const dateInput = document.querySelector(".task-date")

    titleInput.value = task.title
    contentInput.value = task.content
    categoryInput.value = task.category
    if (task.deadline) {
        const deadline = task.deadline
        const year = deadline.getFullYear()
        const month = String(deadline.getMonth() + 1).padStart(2, "0")
        const day = String(deadline.getDate()).padStart(2, "0")
        dateInput.value = `${year}-${month}-${day}`
    }
}

function reset_top_bar() {
    const top_bar_element = document.querySelector(".topbar")
    top_bar_element.id = ""

    const delete_task_btn = document.getElementById("delete_task_btn")
    delete_task_btn.classList.add("invisible")

    const update_task_btn = document.getElementById("update_task_btn")
    update_task_btn.classList.add("invisible")

    const newTaskBtn = document.querySelector(".new-task-btn")
    newTaskBtn.classList.remove("btn-disabled")
    newTaskBtn.disabled = false

    document.querySelector(".task-name").value = ""
    document.querySelector(".task-content").value = ""
    document.querySelector(".task-category").value = ""
    document.querySelector(".task-date").value = ""
}

function map_to_task(data) {
    return {
        id: data.id,
        title: data.title,
        deadline: data.start,
        content: data.extendedProps.content,
        category: data.extendedProps.category
    }
}

function error_message(message) {
    alert(message);
}

async function get(url, error_message) {
    const token = sessionStorage.getItem("token");

    const response = await fetch(url, {
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error(error_message);
    }

    return await response.json();
}

window.refresh_calendar = refresh_calendar;

setup_calendar();
