window.onload = function () {
    var checkBox = document.getElementById("ToSCheckBox");
    var signUpButton = document.getElementById("SignUp_Button");
    checkBox.addEventListener('change', function () {
        signUpButton.disabled = !this.checked;
        toggleButtonHoverEffect(signUpButton.disabled);
    });
    toggleButtonHoverEffect(signUpButton.disabled);
}

function toggleButtonHoverEffect(isDisabled) {
    var signUpButton = document.getElementById("SignUp_Button");
    if (isDisabled) {
        signUpButton.classList.add('disabled-hover');
    } else {
        signUpButton.classList.remove('disabled-hover');
    }
}

function attachValidationHandlers() {
    var Gmail_Validator = document.getElementById("Gmail_Validator");
    var Username_Validator = document.getElementById("Username_Validator");
    if (!Gmail_Validator.isValid) {
        Gmail_Validator.style.display = 'block';
        document.getElementById("PWLabel").style.marginTop = '18px';
    }
    if (!Username_Validator.isValid) {
        Username_Validator.style.display = 'block';
        document.getElementById("Gmail_Label").style.marginTop = '18px';
    }
}