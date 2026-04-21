function is_tovholder() {
    const token = sessionStorage.getItem("token");
    if (!token) return false;

    try {
        const decoded = jwt_decode(token);

        const roles =
            decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
            || decoded["role"];

        return Array.isArray(roles)
            ? roles.includes("tovholder")
            : roles === "tovholder";

    } catch (e) {
        console.error("Token decode failed:", e);
        return false;
    }
}