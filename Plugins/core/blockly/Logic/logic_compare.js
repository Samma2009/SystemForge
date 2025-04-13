(block) => {
    const OPERATORS = {
        'EQ': '==',
        'NEQ': '!=',
        'LT': '<',
        'LTE': '<=',
        'GT': '>',
        'GTE': '>='
    };
    const operator = OPERATORS[block.getFieldValue('OP')];
    const order = operator === '==' || operator === '!=' ? javascript.Order.EQUALITY : javascript.Order.RELATIONAL;
    const argument0 = Gen.valueToCode(block, 'A', order) || '0';
    const argument1 = Gen.valueToCode(block, 'B', order) || '0';
    const code = argument0 + ' ' + operator + ' ' + argument1;
    return [code, order];
}