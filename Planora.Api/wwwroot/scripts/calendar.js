const url = "api/task"
const read_fail = "Failed to read tasks"

async function setup_calendar() {
    try {
        const data = await get(url, read_fail)
        const events = map_to_event(data)
        create_calendar(events)
    } catch (error) {
        error_message(error.message)
    }
}

async function get(url, error_message) {
    const token = sessionStorage.getItem("token");

    const response = await fetch("/api/Task", {
        headers: {
            "Authorization": `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error(error_message);
    }

    return await response.json();
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
/* All tasks in calendar => events must follow this format
{
  id: '1',               // Optional: useful for lookups
  title: 'Finish Report', // Required: what appears on the calendar
  start: '2026-04-20',   // Required: ISO8601 string or Date object
  end: '2026-04-21',     // Optional: for multi-day events
  allDay: true           // Optional: defaults to true for date strings
}
*/
function create_calendar(tasks) {
    const calendarElement = document.getElementById("calendar")

    if (!calendarElement) {
        return
    }

    const calendar = new FullCalendar.Calendar(calendarElement, {
        initialView: 'listMonth',
        events: tasks,
        eventClick: task_click_handler
    })

    calendar.render();
}

function task_click_handler(info) {
    const data = info.event
    const task = map_to_task(data)

    const topbar_element = document.querySelector(".topbar")
    topbar_element.id = task.id

    const delete_task_btn = document.getElementById("delete_task_btn")
    delete_task_btn.classList.remove("invisible")

    const newTaskBtn = document.querySelector(".new-task-btn")
    
    const titleInput = document.querySelector(".task-name")
    const contentInput = document.querySelector(".task-content")
    const categoryInput = document.querySelector(".task-category")
    const dateInput = document.querySelector(".task-date")

    titleInput.value = task.title
    contentInput.value = task.content
    categoryInput.value = task.category
    if (task.deadline) {
        dateInput.value = new Date(task.deadline).toISOString().split("T")[0]
    }
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
    alert(message)
}

setup_calendar()