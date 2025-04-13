(block) => {
    let text = block.getFieldValue("COLOUR");
    return [`${text.toUpperCase().replace("#","0x")}`,javascript.Order.ATOMIC];
}