let editorInstance;

window.initializeCKEditor = (editorId) => {
    ClassicEditor
        .create(document.querySelector(`#${editorId}`))
        .then(editor => {
            editorInstance = editor;
        })
        .catch(error => {
            console.error(error);
        });
};

window.getEditorData = () => {
    if (editorInstance) {
        return editorInstance.getData();
    } else {
        console.error('Editor instance is not defined.');
        return '';
    }
};