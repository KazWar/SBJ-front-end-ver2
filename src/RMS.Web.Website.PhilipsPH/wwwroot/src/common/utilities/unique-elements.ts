/**
 * Returns unique elements from the specified array
 * @param items - Elements to select from
 * @param valueSelector - Property name or function extracting value from array elements.
 * If not specified, array elements are treated as values.
 * @returns Unique elements from the specified array
 */

export function uniqueElements (items:any, valueSelector:any) {
  if (items) {
    const values:any = []
    const valueFn = valueSelector
      ? (typeof valueSelector === 'function' ? (item:any) => valueSelector(item) : (item:any) => item[valueSelector.toString()])
      : (item:any) => item
    return items.filter((item:any) => {
      const value = valueFn(item)
      if (!values.includes(value)) {
        values.push(value)
        return item
      }
    })
  }
}