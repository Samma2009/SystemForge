(block) => {
    const varr = workspace.getVariableById(block.getFieldValue('var'));
    const v = varr.type[0] + varr.name;
    const s = block.getFieldValue("start");
    const cond = Gen.valueToCode(block, 'condition', javascript.Order.ATOMIC);
    var code = `for (${v} = ${s}; ${v} < ${cond}; ${v}++) {\n`;
    var nextBlock = block.getInputTargetBlock('code');
    while (nextBlock) {
        var tempCode = Gen.blockToCode(nextBlock);
        if (Gen.STATEMENT_PREFIX) {
            tempCode = Gen.prefixLines(
                Gen.STATEMENT_PREFIX.replace(/%1/g,
                    '\'' + nextBlock.id + '\'') + '\n', Gen.INDENT) + tempCode;
        }
        code += tempCode;
        nextBlock = nextBlock.getNextBlock();
    }
    code += '}\n';
    return code;
}