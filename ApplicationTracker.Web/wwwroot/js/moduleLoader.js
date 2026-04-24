export async function loadModules(names) {
    // Defensive: convert string to array if necessary
    if (typeof names === "string") {
        names = [names];
    }

    const result = {};

    for (const name of names) {
        const mod = await import(`./${name}.js`);
        Object.assign(result, mod);
    }

    return result;
}