(block) => {
    let text = block.getFieldValue("Value");
    return [`"${text}"`,javascript.Order.ATOMIC];
}