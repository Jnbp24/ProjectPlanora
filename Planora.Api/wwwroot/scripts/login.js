document.addEventListener("keydown", (event) => {
	if (event.key === "Enter") {
		event.preventDefault();
		login();
	}
});

async function login() {
	try {
		const data = create_data()
		await post(data)
		window.location.href = "/dashboard.html"
	} catch (error) {
		error_message(error.message)
		clear_input()
	}
}

function create_data() {
	const email = document.getElementById("email").value.trim()
	const password = document.getElementById("password").value.trim()

	validateInput(email, password)

	return {
		email: email,
		password: password
	}
}

function validateInput(email, password) {
	if (!email || !password) {
		throw new Error("Fill out all required fields")
	}
	const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
	if (!emailPattern.test(email)) {
		throw new Error("Invalid email format")
	}
}

async function post(data) {
	const response = await fetch("/api/auth/login", {
		method: "POST",
		headers: {
			"Content-Type": "application/json"
		},
		body: JSON.stringify(data)
	});
	if (!response.ok || !response.success) {
		throw new Error(response.error)
	}
	sessionStorage.setItem("token", response.token)
}

function clear_input() {
	document.getElementById("email").value = ""
	document.getElementById("password").value = ""
}
function error_message(message) {
	alert(message)
}