// newUser_password.js
const form = document.getElementById('new-password-form');
const oldPasswordInput = document.getElementById('old-password');
const newPasswordInput = document.getElementById('new-password');
const confirmInput = document.getElementById('confirm-password');
const errorDiv = document.getElementById('error-message');
const successDiv = document.getElementById('success-message');

form.addEventListener('submit', async function (e) {
    e.preventDefault();
    errorDiv.textContent = '';
    successDiv.textContent = '';

    const oldPassword = oldPasswordInput.value.trim();
    const newPassword = newPasswordInput.value.trim();
    const confirm = confirmInput.value.trim();

    if (!oldPassword || !newPassword || !confirm) {
        errorDiv.textContent = 'Please fill in all fields.';
        return;
    }
    if (newPassword.length < 8) {
        errorDiv.textContent = 'New password must be at least 8 characters.';
        return;
    }
    if (newPassword !== confirm) {
        errorDiv.textContent = 'New passwords do not match.';
        return;
    }

    const params = new URLSearchParams(window.location.search);
    const email = params.get('email');

    if (!email) {
        errorDiv.textContent = 'Email not found. Invalid request.';
        return;
    }

    try {
        const response = await fetch('/api/auth/password-change', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                "Email": email,
                "OldPassword": oldPassword,
                "NewPassword": newPassword,
                "ConfirmPassword": confirm
            })
        });

        if (response.ok) {
            successDiv.textContent = 'Your password has been changed successfully.';
            form.reset();
            // Redirect to index.html after 1.5 seconds
            setTimeout(() => {
                window.location.href = '/';
            }, 1500);
        } else {
            const errorData = await response.json().catch(() => ({}));
            errorDiv.textContent = errorData.message || 'Failed to change password. Please try again.';
        }
    } catch (err) {
        errorDiv.textContent = 'Network error. Please try again.';
    }
});
