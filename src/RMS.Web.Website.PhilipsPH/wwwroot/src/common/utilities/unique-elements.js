/**
 * Returns unique elements from the specified array
 * @param {Array} items Elements to select from
 * @param {String|Function} valueSelector Property name or function extracting value from array elements.
 * If not specified, array elements are treated as values.
 * @returns {Array} Unique elements from the specified array
 */
export function uniqueElements(items, valueSelector) {
    if (items) {
        const values = [];
        const valueFn = valueSelector
            ? (typeof valueSelector === 'function' ? (item) => valueSelector(item) : (item) => item[valueSelector.toString()])
            : (item) => item;
        return items.filter((item) => {
            const value = valueFn(item);
            if (!values.includes(value)) {
                values.push(value);
                return item;
            }
        });
    }
}
//# sourceMappingURL=unique-elements.js.map