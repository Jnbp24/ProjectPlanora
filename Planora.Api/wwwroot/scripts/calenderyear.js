document.addEventListener("DOMContentLoaded", loadCalenderYears);

const COLORS = [
    "#2f80ed", "#27ae60", "#e67e22", "#8e44ad",
    "#e74c3c", "#16a085", "#d35400", "#2980b9"
];

async function loadCalenderYears() {
    const token = sessionStorage.getItem("token");

    const res = await fetch("api/calenderyear", {
        headers: {
            "Authorization": `Bearer ${token}`
        }
    });

    if (!res.ok) return;

    const calenderYears = await res.json();
    const list = document.getElementById("calenderyear-list");
    const empty = document.getElementById("calenderyear-empty");
    const template = document.getElementById("calenderyear-card-template");

    if (calenderYears.length > 0) empty.style.display = "none";

    calenderYears.forEach((calenderYear, index) => {
        const card = template.content.cloneNode(true).querySelector(".calenderyear-card");

        card.style.background = COLORS[index % COLORS.length];

        card.querySelector(".calenderyear-card-title").textContent =
            calenderYear.title;

        card.addEventListener("click", () => {
            window.location.href = `dashboard.html?calenderYearId=${calenderYear.calenderYearId}`;
        });

        list.appendChild(card);
    });
}