(block) => {
    var code = 'void entry(struct limine_framebuffer *fb) {\n\tframebuffer = fb;\n\tfb_ptr = framebuffer->address;\n\t';
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