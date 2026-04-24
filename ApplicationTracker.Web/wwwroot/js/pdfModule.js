//export async function setIframeSrc(iframeEl, url) {
//    if (!iframeEl) return;
//    iframeEl.src = `data:application/pdf;base64,${url}`;
//} ;


export async function setIframeSrc(iframeEl, pdfBase64) {
    if (!iframeEl) return;
    const byteCharacters = atob(pdfBase64);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: 'application/pdf' });
    const blobUrl = URL.createObjectURL(blob);
    iframeEl.src = blobUrl;
}
