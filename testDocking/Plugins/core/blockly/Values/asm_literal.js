(block) => {
    let text = block.getFieldValue("Value");
    return `asm("${text}");`;
}