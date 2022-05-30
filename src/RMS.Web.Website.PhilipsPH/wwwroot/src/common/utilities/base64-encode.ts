/**
 * Takes in a file and converts it to base 64 encoding
 * 
 * @param file - Image file
 * @returns A base 64 encoded file
 */
 export function Base64Encode (file:File): Promise<string| ArrayBuffer | null> {
    return new Promise ((resolve, reject) => {
        const reader = new FileReader()
        reader.readAsDataURL(file)
        reader.onloadend = () => {
            resolve(reader.result)
        }
    })
}