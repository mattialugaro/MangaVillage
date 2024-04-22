const passwordInput = document.getElementById("password");
const showPasswordButton = document.getElementById("showPassword");

showPasswordButton.addEventListener("click", () => {
    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        showPasswordButton.textContent = "Nascondi";
    } else {
        passwordInput.type = "password";
        showPasswordButton.textContent = "Mostra";
    }
});
