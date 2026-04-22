// reset_password.js
const form = document.getElementById('reset-password-form');
const passwordInput = document.getElementById('password');
const confirmInput = document.getElementById('confirm-password');
const errorDiv = document.getElementById('reset-error');
const successDiv = document.getElementById('reset-success');

form.addEventListener('submit', async function (e) {
    e.preventDefault();
    errorDiv.textContent = '';
    successDiv.textContent = '';

    const password = passwordInput.value.trim();
    const confirm = confirmInput.value.trim();

    if (!password || !confirm) {
        errorDiv.textContent = 'Please fill in all fields.';
        return;
    }
    if (password.length < 8) {
        errorDiv.textContent = 'Password must be at least 8 characters.';
        return;
    }
    if (password !== confirm) {
        errorDiv.textContent = 'Passwords do not match.';
        return;
    }

    const params = new URLSearchParams(window.location.search);

    const email = params.get('email'); 
    const token = params.get('token');

    try {
        // Replace URL with your backend endpoint
        const response = await fetch('/api/auth/reset', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(
                {   
                    "Email": email, 
                    "Token": token,  
                    "NewPassword": password,
                    "ConfirmPassword": confirm
                })
        });
        if (response.ok) {
            successDiv.textContent = 'Your password have been changed';

            form.reset();
        } else {
            errorDiv.textContent = 'Failed to set new password. Please try again.';
        }
    } catch (err) {
        errorDiv.textContent = 'Network error. Please try again.';
    }

    form.reset();
});
