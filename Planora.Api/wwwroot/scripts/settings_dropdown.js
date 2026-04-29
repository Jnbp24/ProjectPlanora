document.addEventListener('DOMContentLoaded', function() {
    const settingsBtn = document.querySelector('.settings-btn');
    const settingsDropdown = document.querySelector('.settings-dropdown');
    const settingsContainer = document.querySelector('.settings-dropdown-container');
    const changePasswordLink = document.getElementById('change-password-link');
    let hideTimeout;

    // Extract email from JWT token
    function getEmailFromToken() {
        const token = sessionStorage.getItem('token');
        if (!token) {
            console.warn('No token found in session storage');
            return null;
        }

        try {
            const decoded = jwt_decode(token);
            // Common email claim names in JWT tokens
            const email = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress']
                || decoded['email']
                || decoded['sub']
                || null;
            return email;
        } catch (e) {
            console.error('Token decode failed:', e);
            return null;
        }
    }

    // Set the password change link with email
    function initializePasswordLink() {
        const email = getEmailFromToken();
        if (email) {
            changePasswordLink.href = `password_change.html?email=${encodeURIComponent(email)}`;
        } else {
            changePasswordLink.href = 'password_change.html';
        }
    }

    // Toggle dropdown on button click
    settingsBtn.addEventListener('click', function(e) {
        e.stopPropagation();
        settingsDropdown.classList.toggle('active');
    });

    // Show dropdown on hover
    settingsBtn.addEventListener('mouseenter', function() {
        clearTimeout(hideTimeout);
        settingsDropdown.classList.add('active');
    });

    // Show dropdown on hover over the dropdown itself
    settingsDropdown.addEventListener('mouseenter', function() {
        clearTimeout(hideTimeout);
        settingsDropdown.classList.add('active');
    });

    // Hide dropdown when mouse leaves the container (with delay)
    settingsContainer.addEventListener('mouseleave', function() {
        hideTimeout = setTimeout(function() {
            settingsDropdown.classList.remove('active');
        }, 150);
    });

    // Close dropdown when clicking on a link
    changePasswordLink.addEventListener('click', function() {
        settingsDropdown.classList.remove('active');
    });

    // Close dropdown when clicking elsewhere
    document.addEventListener('click', function(e) {
        if (!settingsContainer.contains(e.target)) {
            settingsDropdown.classList.remove('active');
        }
    });

    // Initialize on page load
    initializePasswordLink();
});
