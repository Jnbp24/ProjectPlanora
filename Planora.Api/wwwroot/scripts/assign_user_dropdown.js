let assigned_users;

async function create_task_user_selection() {
	const users = await get("/api/user");

	assigned_users = new TomSelect('#task-assignees', {
		plugins: ['remove_button', 'checkbox_options'],
		maxOptions: null,
		placeholder: 'Assign to…',
		options: users.map(user => ({ value: user.userId, text: `${user.firstName} ${user.lastName}` })),
	});
}

create_task_user_selection()