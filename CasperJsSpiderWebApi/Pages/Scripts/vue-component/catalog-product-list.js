// JavaScript source code
Vue.component('catalog-product-list', {
    data: function () {
        var date = new Date()
        //获取数据 
        return {
            loading: false,
            tableData: [],
            total: 0,
            isShowTotal: false,
            queryForm: {
                Name: '',
                DatePeriod: [new Date(date.setTime(date.getTime() - 3600 * 1000 * 24 * 7)), new Date()]
                //StartDate: new Date(date.setMonth(date.getMonth() - 1)),
                //EndDate: new Date(),
            },
            pickerOptions2: {
                shortcuts: [{
                    text: '最近一周',
                    onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近一个月',
                    onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近三个月',
                    onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
                        picker.$emit('pick', [start, end]);
                    }
                }]
            }
        } 
    },
    template: '<el-container  style="height:100%;width:100%;">\
                <el-header style="height:85px;">\
                    <el-form :model="queryForm" label-width="80px" @submit.native.prevent>\
                        <el-row>\
                            <el-col :span="5">\
                                <el-form-item label="产品名称">\
                                    <el-input v-model="queryForm.Name" clearable placeholder="产品名称"></el-input>\
                                </el-form-item>\
                            </el-col>\
                            <el-col :span="10">\
                                <el-form-item label="日期段">\
                                     <el-date-picker type="daterange" v-model="queryForm.DatePeriod"  align="right" unlink-panels range-separator="至" placeholder="开始日期" end-placeholder="结束日期" :picker-options="pickerOptions2" style = "width: 100%;" ></el-date-picker>\
                                </el-form-item>\
                            </el-col>\
                            <el-col :span="9">\
                                <el-form-item>\
                                    <el-button type="primary" @click="onSubmit" >查询</el-button>\
                                </el-form-item>\
                            </el-col>\
                        </el-row>\
                        <el-row v-if="isShowTotal">\
                            <el-col :span="24">当前搜索值：<span style="color:red;margin-right:10px">{{queryForm.Name}}</span>出现总次数：<span style="color:green">{{total}}</span></el-col>\
                        </el-row>\
                    </el-form>\
                </el-header>\
                <el-main style="height:100%;width:100%;">\
                 <el-table :v-loading="loading" :data="tableData" max-height="100%" height="100%"  border >\
                   <el-table-column type="index" width="50" ></el-table-column>\
                   <el-table-column label="产品名" ><template slot-scope="scope"><div v-html="scope.row.ProductName"></div></template></el-table-column>\
                   <el-table-column  label="图片" ><template slot-scope="scope"><img style="max-height:100px;max-width:100px;" :src="scope.row.ProductImgPath"></template></el-table-column>\
                   <el-table-column prop="TotalCount" label="出现次数"  ></el-table-column>\
                 </el-table>\
                </el-main>\
              </el-container>'
    ,
    methods: {
        createView: function () {
            this.$root.navselected = this.$route.query.index;
            this.queryForm.Name = '';
            this.loadPage();
        },
        loadData: function () {
            this.loading = true, params = {
                CatalogID: this.$route.query.id,
                Name: this.queryForm.Name
            };
            if (this.queryForm.DatePeriod
                && this.queryForm.DatePeriod.length > 1) {
                params.StartDate =this.dateFormat(this.queryForm.DatePeriod[0]);
                params.EndDate = this.dateFormat(this.queryForm.DatePeriod[1], 23, 59, 59);
            }
            googleBrower.GetCatalogProductList(JSON.stringify(params), function (json) {
                var rs = JSON.parse(json), data, queryName, isQueryName;
                if (rs.Success) {
                    queryName = this.queryForm.Name.trim();
                    isQueryName = queryName.length > 0;
                    data = rs.Data;
                    if (isQueryName) {
                        this.tableData = data.map(function (item) {
                            item.ProductName = item.ProductName.replace(queryName, "<span style='color:red;'>" + queryName + "</span>");
                            return item;
                        });
                    } else {
                        this.tableData = data;
                    }
                    //this.tableData = rs.Data;
                    this.total = rs.Total;
                    this.isShowTotal = isQueryName;
                    this.loading = false;
                } else {
                    this.$message.error(rs.Msg);
                }
            }.bind(this));

        },
        loadPage: function () {
            this.loadData()
        },
        onSubmit: function () {
            this.loadPage();
        },
        dateFormat: function (date, hours, min, second, ms) {
            return new Date(date.getFullYear(), date.getMonth(), date.getDate(), hours || 0, min || 0, second || 0, ms || 0);
        }
    },
    created: function () {
        this.createView();
    },
    watch: {
        $route: function () {
            this.createView();
        }
    }
});