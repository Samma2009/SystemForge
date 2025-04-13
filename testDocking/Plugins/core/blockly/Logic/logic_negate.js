(block) => {
    const order = javascript.Order.LOGICAL_NOT;
  const argument0 = Gen.valueToCode(block, 'BOOL', order) || 'true';
  const code = '!' + argument0;
  return [code, order];
}