(block) => {
    let n = 0;
    let code = '';
    if (Gen.STATEMENT_PREFIX) {
        code += Gen.injectId(Gen.STATEMENT_PREFIX, block);
    }
    do {
        const conditionCode = Gen.valueToCode(block, 'IF' + n, javascript.Order.NONE) || '0';
        let branchCode = Gen.statementToCode(block, 'DO' + n);
        if (Gen.STATEMENT_SUFFIX) {
            branchCode = Gen.prefixLines(Gen.injectId(Gen.STATEMENT_SUFFIX, block), Gen.INDENT) + branchCode;
        }
        code += (n > 0 ? ' else ' : '') + 'if (' + conditionCode + ') {\n' + branchCode + '}';
        n++;
    } while (block.getInput('IF' + n));

    if (block.getInput('ELSE') || Gen.STATEMENT_SUFFIX) {
        let branchCode = Gen.statementToCode(block, 'ELSE');
        if (Gen.STATEMENT_SUFFIX) {
            branchCode = Gen.prefixLines(Gen.injectId(Gen.STATEMENT_SUFFIX, block), Gen.INDENT) + branchCode;
        }
        code += ' else {\n' + branchCode + '}';
    }
    return code + '\n';
}