/**
 * Create a function to take in the new value and emit it
 *
 * @param value new model value
 */
export function Base64Encode(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onloadend = () => {
            resolve(reader.result);
        };
    });
}
//# sourceMappingURL=base64-encode.js.map