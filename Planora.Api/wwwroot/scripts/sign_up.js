
async function sign_up() {
	try {
		const data = create_data()
		await post(data)	
		clear_input()
	} catch (error) {
		error_message(error.message)
	} 
}

function create_data() {
	const firstname = document.getElementById("firstName").value.trim()
	const lastname = document.getElementById("lastName").value.trim()
	const email = document.getElementById("email").value.trim()
	const tovholder = document.getElementById("tovholder").checked

	validateInput(firstname, lastname, email)

	return {
		firstname: firstname,
		lastname: lastname,
		email: email,
		tovholder: tovholder
	}
}

function validateInput(firstname, lastname, email) {
	if (!firstname || !lastname || !email) {
		throw new Error("Fill out all required fields")
	}
	const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
	if (!emailPattern.test(email)) {
		throw new Error("Invalid email format");
	}
}

async function post(data) {
	const token = sessionStorage.getItem("token");

	const response = await fetch("/api/user", {
		method: "POST",
		headers: {
			"Content-Type": "application/json",
			"Authorization": `Bearer ${token}`
		},
		body: JSON.stringify(data)
	});
	if (response.ok) {
		error_message("User successfully created")
	}
	if (response.status === 409) {
		const error = await response.json()
		error_message(error.message)
		return
	}
	if (!response.ok) {
		error_message("Unexpected sign up error")
	}
}

function clear_input() {
	document.getElementById("firstName").value = "";
	document.getElementById("lastName").value = "";
	document.getElementById("email").value = "";
	document.getElementById("tovholder").checked = false;
}
function error_message(message) {
	alert(message)
}