
(() => {

    console.log("脚本已加载");

    const accountText = ref("");
    const accountCount = ref(0);
    const isDragOver = ref(false);
    const folderFiles = ref([]);

    //导入结果
    const importResult = ref([]);

    const separatorRegex = /[,， \t]|----/;

    /**
     * 读取账号文件
     * @param file
     */
    function readAccountFromFile(file) {
        const reader = new FileReader();
        reader.onload = (e) => {
            accountText.value = e.target.result;
        };
        reader.onerror = () => {
            alert("文件读取失败");
        };
        reader.readAsText(file);
    }

    /**
     * 拖拽账号文件导入
     */
    function onAccountFileDrop(event) {
        isDragOver.value = false;
        const files = event.dataTransfer.files;
        if (files.length > 0) {
            readAccountFromFile(files[0]);
        }
    }

    /**
     * 选择账号文件导入
     * @param event
     */
    function onSelectAccounts(event) {
        const files = event.target.files;
        if (files.length > 0) {
            readAccountFromFile(files[0]);
        }
    }

    /**
     * 选择令牌文件夹
     */
    async function onSelectMaFiles(event) {
        const files = Array.from(event.target.files);

        if (files.length > 0) {
            // 保存文件列表
            folderFiles.value = files;
        } else {
            folderFiles.value = [];
        }
    }

    /**
     * 更新账号数量
     */
    function updateAccountCount() {
        if (!accountText.value) {
            accountCount.value = 0;
            return;
        }

        let count = 0;
        const lines = accountText.value.split("\n");

        for (const line of lines) {
            if (!line.trim()) {
                continue;
            }

            const parts = line.split(separatorRegex).filter((x) => x);
            if (parts.length >= 2) {
                count++;
            }
        }

        accountCount.value = count;
    }

    /**
     * 解析账号信息
     * @param text
     */
    function parseAccounts() {
        const lines = accountText.value.split("\n");
        const result = [];

        for (const line of lines) {
            if (!line.trim()) {
                continue;
            }

            const parts = line.split(separatorRegex).filter((x) => x);

            if (parts.length >= 2) {
                const username = parts[0].trim();
                const password = parts[1].trim();

                result.push({
                    username,
                    password,
                    identitySecret: null,
                    sharedSecret: null,
                });
            }
        }

        return result;
    }

    /**
     * 读取令牌文件
     * @param file
     */
    function readJsonFile(file) {
        return new Promise((resolve) => {
            const reader = new FileReader();
            reader.onload = (e) => {
                try {
                    const json = JSON.parse(e.target.result);
                    resolve(json);
                } catch {
                    resolve(null);
                }
            };
            reader.onerror = () => {
                resolve(null);
            };
            reader.readAsText(file);
        });
    }

    /**
     * 执行导入操作
     */
    async function doImport() {
        importResult.value = [];

        if (folderFiles.value.length === 0) {
            alert("请先选择文件夹");
            return;
        }

        if (!accountText.value.trim()) {
            alert("请先输入账号信息");
            return;
        }

        // 解析账号信息
        const parsedAccounts = parseAccounts(accountText.value);

        // 创建文件名到文件的映射
        const fileMap = {};
        for (const file of folderFiles.value) {
            const fileName = file.name.toString().toLowerCase();
            fileMap[fileName] = file;
        }

        // 为每个账号查找对应的 JSON 文件
        for (const account of parsedAccounts) {
            const fileName = account.username + ".mafile";
            const jsonFile = fileMap[fileName];
            if (!jsonFile) {
                continue;
            }

            const jsonData = await readJsonFile(jsonFile);
            if (jsonData) {
                const { identity_secret, shared_secret } = jsonData;
                account.identitySecret = identity_secret ?? null;
                account.sharedSecret = shared_secret ?? null;
            }
        }

        importResult.value = parsedAccounts;
    }

})();
