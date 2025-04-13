(block) => {
    let text = block.getFieldValue("NAME");
    if (Number.isInteger(text)) {
        return [`${text}`,javascript.Order.ATOMIC];
    }
    else {
        return [`${text}f`,javascript.Order.ATOMIC];
    }
}