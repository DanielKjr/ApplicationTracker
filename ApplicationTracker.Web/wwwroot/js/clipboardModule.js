export async function copyTextToClipboard(text) {
    if (navigator.clipboard) {
        try {
            await navigator.clipboard.writeText(text);
            return true;
        } catch (e) {
            console.warn('Clipboard write failed', e);
            return false;
        }
    }
    return false;
}