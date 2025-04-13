(block) => {
    var functionName = block.getFieldValue('NAME').replace(' ','_').replace('.','__').replace(':','___');
    var code = `void ${functionName}() {\n`;
    var nextBlock = block.getInputTargetBlock('STACK');
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