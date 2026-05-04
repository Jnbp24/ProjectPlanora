let calendar;

async function setup_calendar() {
    try {
        const tasks = await get("api/task/extended")
        if (!tasks) {
            throw new Error("failed to load tasks")
        }

        const params = new URLSearchParams(window.location.search);
        const calenderYearId = params.get("calenderYearId");

        const filteredTasks = calenderYearId
            ? tasks.filter(t => t.calenderYearId === calenderYearId)
            : tasks;

        const events = map_to_event(filteredTasks);
        create_calendar(events)

        if (filteredTasks.length > 0 && filteredTasks[0].deadline) {
            const firstDate = new Date(filteredTasks[0].deadline);
            calendar.gotoDate(firstDate);
        }

    } catch (error) {
        error_message(error.message)
    }
}

async function refresh_calendar() {
    try {
        const tasks = await get_tasks_with_category();

        if (!calendar) return;

        const params = new URLSearchParams(window.location.search);
        const calenderYearId = params.get("calenderYearId");

        const filteredTasks = calenderYearId
            ? tasks.filter(t => t.calenderYearId === calenderYearId)
            : tasks;

        calendar.removeAllEvents();

        calendar.addEventSource(map_to_event(filteredTasks));

        if (filteredTasks.length > 0 && filteredTasks[0].deadline) {
            calendar.gotoDate(new Date(filteredTasks[0].deadline));
        }

    } catch (error) {
        error_message(error.message);
    }
}

function map_to_event(tasks) {
    return tasks.map(task => ({
        id: task.taskId,
        title: task.title,
        start: task.deadline,
        extendedProps: {
            content: task.content,
            category: task.category,
            users: task.users,
            done: task.done
        },
        backgroundColor: task.category?.hexColor ?? '#6b7280',
        borderColor: task.category?.hexColor ?? '#6b7280',
        allDay: true
    }))
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
                    <div>
                        <span class="event-element" id="event-category" style="--category-color: ${arg.event.extendedProps.category.hexColor}">
                            ${arg.event.extendedProps.category.name}
                        </span>
                        <br>
                        <br>
                        <span class="event-element">
                            ${arg.event.title ?? 'No title'}
                        </span>
                        <br>
                        <br>
                        <span class="event-element">
                            ${arg.event.extendedProps.content ?? 'No content'}
                        </span>
                        <br>
                        <br>
                        <span class="event-element">
                            ${arg.event.extendedProps.users?.length > 0
                                ? arg.event.extendedProps.users.map(u => `${u.firstName} ${u.lastName}`).join(', ')
                                : 'Non assigned'}
                        </span>
                        <br><br>
                        <label class="event-done-label">
                            <input type="checkbox" class="event-done-checkbox" data-task-id="${arg.event.id}" ${arg.event.extendedProps.done ? 'checked' : ''}>
                            Done
                        </label>
                    </div>
                `
            }
        }
    })

    calendar.render();
    calendar.el.addEventListener("change", async (e) => {
        if (!e.target.classList.contains("event-done-checkbox")) return;

        const taskId = e.target.dataset.taskId;
        const done = e.target.checked;
        const token = sessionStorage.getItem("token");

        const res = await fetch(`api/task/${taskId}/completed`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(done)
        });

        if (!res.ok) e.target.checked = !e.target.checked;
    });
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
    categoryInput.value = task.category?.categoryId ?? ""
    categoryInput.dispatchEvent(new Event("change"))
    if (task.deadline) {
        const deadline = task.deadline
        const year = deadline.getFullYear()
        const month = String(deadline.getMonth() + 1).padStart(2, "0")
        const day = String(deadline.getDate()).padStart(2, "0")
        dateInput.value = `${year}-${month}-${day}`
    }
    assigned_users.clear()
    assigned_users.setValue(task.assigned_users.map(user => String(user.userId)))
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
    const categoryInput = document.querySelector(".task-category")
    categoryInput.value = ""
    categoryInput.style.color = ""
    categoryInput.style.background = ""
    categoryInput.style.fontWeight = ""
    document.querySelector(".task-date").value = ""
}

function map_to_task(data) {
    return {
        id: data.id,
        title: data.title,
        deadline: data.start,
        content: data.extendedProps.content,
        category: data.extendedProps.category,
        assigned_users: data.extendedProps.users
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
