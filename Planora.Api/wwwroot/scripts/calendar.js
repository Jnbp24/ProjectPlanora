document.addEventListener("DOMContentLoaded", function () {
    const container = document.getElementById("timeline");

    // Create dataset
    const items = new vis.DataSet([
        {
            id: 1,
            content: "Task A",
            start: "2026-04-10",
            end: "2026-04-12"
        },
        {
            id: 2,
            content: "Task B",
            start: "2026-04-11",
            end: "2026-04-13"
        }
    ]);

    // Configuration options
    const options = {
        editable: true,        // drag & resize tasks
        stack: true,           // stack overlapping items
        zoomable: true,
        moveable: true
    };

    // Create timeline
    const timeline = new vis.Timeline(container, items, options);
});