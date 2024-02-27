function expandText(element) {
    var fullTextElement = element.nextElementSibling;
    var itemTemplate = element.closest('.item-template');
    if (fullTextElement.style.display === "none") {
        fullTextElement.style.display = "inline";
        element.style.display = "none";
        itemTemplate.style.height = "auto";
    } else {
        fullTextElement.style.display = "none";
        element.style.display = "inline";
        itemTemplate.style.height = "225px";
    }
}