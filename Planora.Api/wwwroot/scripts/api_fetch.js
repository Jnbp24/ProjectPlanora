const USER_API = "/api/user"
const TASK_API = "/api/task"
const CATEGORY_API = "/api/category"
const CALENDER_YEAR_API = "/api/calenderyear"
const AUTH_API = "api/auth"

async function apiFetch(url, options = {}) {
    const token = sessionStorage.getItem("token")

    const response = await fetch(url, {
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`,
            ...options.headers,
        },
        ...options,
    });

    if (response.status === 401) {
        sessionStorage.removeItem("token");
        window.location.href = "/login";
        throw new Error("Unauthorized");
    }

    const result = await response.json().catch(() => null);

    if (!response.ok) {
        const error = new Error(result.error || `HTTP ${response.status}`);
        error.status = response.status;
        error.data = result;
        throw error;
    }

    return result;
}