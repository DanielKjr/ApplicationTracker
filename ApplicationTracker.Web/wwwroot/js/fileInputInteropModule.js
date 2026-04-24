
export async function clearFileInput(id) {
    const element = document.getElementById(id);
    if (element) {
        try {
            element.value = '';
        } catch (e) {
            console && console.warn && console.warn('clearFileInput failed', e);
        }
    }
}


