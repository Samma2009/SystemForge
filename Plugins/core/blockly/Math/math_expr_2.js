(block) => {
    const value_op1 = Gen.valueToCode(block, 'op1', javascript.Order.ATOMIC);
    const dropdown_op = block.getFieldValue('op');
    const value_op2 = Gen.valueToCode(block, 'op2', javascript.Order.ATOMIC);
    return [`${value_op1} ${dropdown_op} ${value_op2}`,javascript.Order.ATOMIC];
}