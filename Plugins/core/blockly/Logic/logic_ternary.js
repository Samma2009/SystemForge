(block) =>  {
    const value_if =
    generator.valueToCode(block, 'IF', javascript.Order.CONDITIONAL) || '0';
  const value_then =
    generator.valueToCode(block, 'THEN', javascript.Order.CONDITIONAL) || 'NULL';
  const value_else =
    generator.valueToCode(block, 'ELSE', javascript.Order.CONDITIONAL) || 'NULL';
  const code = value_if + ' ? ' + value_then + ' : ' + value_else;
  return [code, javascript.Order.CONDITIONAL];
}