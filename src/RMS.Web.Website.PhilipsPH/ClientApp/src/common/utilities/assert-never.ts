export function AssertNever(x: never): never {
    throw new Error("Unexpected object: " + x)
}