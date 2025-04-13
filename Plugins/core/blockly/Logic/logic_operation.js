(block) =>  {
  const operator = block.getFieldValue('OP') === 'AND' ? '&&' : '||';
  const order = operator === '&&' ? javascript.Order.LOGICAL_AND : javascript.Order.LOGICAL_OR;
  let argument0 = Gen.valueToCode(block, 'A', order);
  let argument1 = Gen.valueToCode(block, 'B', order);
  if (!argument0 && !argument1) {
    argument0 = '0';
    argument1 = '0';
  } else {
    const defaultArgument = operator === '&&' ? '1' : '0';
    if (!argument0) {
      argument0 = defaultArgument;
    }
    if (!argument1) {
      argument1 = defaultArgument;
    }
  }
  const code = '('+argument0 + ') ' + operator + ' (' + argument1+')';
  return [code, order];
}