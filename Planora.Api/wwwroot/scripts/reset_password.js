// reset_password.js
const form = document.getElementById('reset-password-form');
const passwordInput = document.getElementById('password');
const confirmInput = document.getElementById('confirm-password');
const errorDiv = document.getElementById('reset-error');
const successDiv = document.getElementById('reset-success');

form.addEventListener('submit', function (e) {
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
    // Simulate success (replace with real API call)
    successDiv.textContent = 'Your password has been reset!';
    form.reset();
});
