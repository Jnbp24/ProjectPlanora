const url = "api/task";
const read_fail = "Failed to read tasks";

let calendar;

async function setup_calendar() {
    try {
        const data = await get(url, read_fail);
        const tasks = map_to_task(data);

        create_calendar(tasks);
    } catch (error) {
        error_message(error.message);
    }
}

async function refresh_calendar() {
    try {
        const data = await get(url, read_fail);
        const tasks = map_to_task(data);

        if (!calendar) return;
        calendar.removeAllEvents();
        calendar.addEventSource(map_to_task(data));

    } catch (error) {
        error_message(error.message);
    }
}

function map_to_task(data) {
    return data.map(task => ({
        id: task.taskid,
        title: task.title,
        start: task.deadline
    }));
}

/*
FullCalendar event format:
{
  id: '1',
  title: 'Task',
  start: '2026-04-20',
  end: '2026-04-21',
  allDay: true
}
*/

function create_calendar(tasks) {
    const calendarElement = document.getElementById("calendar");

    if (!calendarElement) return;

    calendar = new FullCalendar.Calendar(calendarElement, {
        initialView: 'listMonth',
        events: tasks
    });

    calendar.render();
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