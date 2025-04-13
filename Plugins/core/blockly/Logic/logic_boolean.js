(block) => {
    const code = block.getFieldValue('BOOL') === 'TRUE' ? '1' : '0';
    return [code, javascript.Order.ATOMIC];
}