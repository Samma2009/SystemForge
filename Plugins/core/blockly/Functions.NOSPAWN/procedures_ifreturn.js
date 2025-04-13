(block) => {
    const cond = Gen.valueToCode(block,'CONDITION',javascript.Order.NONE);
    let code = `if (${cond}) `

    if (block.hasReturnValue_) {
        const value = Gen.valueToCode(block,'VALUE',javascript.Order.NONE);
        code += `return ${value};\n`
    } else {
        code += `return;\n`
    }

    return code;
}