const url = "api/task"
const read_fail = "Failed to read tasks"

async function setup_calendar() {
    try {
        const data = await get(url, read_fail)
        const tasks = map_to_task(data)
        create_calendar(tasks)
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

function map_to_task(data) {
    return data.map(task => ({
        id: task.taskId,
        title: task.title,
        start: new Date("2026-05-15"),
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
        events: tasks
    })

    calendar.render();
}

function error_message(message) {
    alert(message)
}

setup_calendar()
