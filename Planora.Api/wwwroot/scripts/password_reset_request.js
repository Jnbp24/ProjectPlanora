// password_reset_request.js

const form = document.getElementById('email-reset-form');
const emailInput = document.getElementById('reset-email');
const errorDiv = document.getElementById('email-reset-error');
const successDiv = document.getElementById('email-reset-success');

form.addEventListener('submit', async function (e) {
    e.preventDefault();
    errorDiv.textContent = '';
    successDiv.textContent = '';

    const email = emailInput.value.trim();
    if (!email) {
        errorDiv.textContent = 'Please enter your email.';
        return;
    }
    // Basic email validation
    if (!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(email)) {
        errorDiv.textContent = 'Please enter a valid email address.';
        return;
    }
    try {
        // Replace URL with your backend endpoint
        const response = await fetch('/api/auth/request-reset', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email })
        });
        if (response.ok) {
            successDiv.textContent = 'If your email exists, a reset link has been sent.';
            form.reset();
        } else {
            errorDiv.textContent = 'Failed to send reset link. Please try again.';
        }
    } catch (err) {
        errorDiv.textContent = 'Network error. Please try again.';
    }
});
