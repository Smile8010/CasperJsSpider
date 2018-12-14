// JavaScript source code
Vue.component('list', {
    //props: {
    //    dataId: {
    //        type: String,
    //        default:''
    //    }
    //},
    data: function () {
        var date = new Date()
        //获取数据 
        return {
            tableData: [],
            total: 0,
            limit: 10, 
            queryForm: {
                Name: '',
                RankNumber: 1,
                StartDate: new Date(date.setMonth(date.getMonth() - 1)),
                EndDate: new Date()
            }
        };
    },
    template: '<el-container style="height:100%;width:100%;">\
                <el-header>\
                    <el-form :model="queryForm" label-width="80px" @submit.native.prevent>\
                        <el-row>\
                            <el-col :span="5">\
                                <el-form-item label="产品名称">\
                                    <el-input v-model="queryForm.Name" clearable placeholder="产品名称"></el-input>\
                                </el-form-item>\
                            </el-col>\
                            <el-col :span="5">\
                                <el-form-item label="产品等级">\
                                    <el-select v-model="queryForm.RankNumber" placeholder="请选择等级">\
                                        <template v-for="n in 100" >\
                                            <el-option :label="n" :value="n"></el-option>\
                                        </template>\
                                    </el-select>\
                                </el-form-item>\
                            </el-col>\
                            <el-col :span="10">\
                                <el-form-item label="日期段">\
                                    <el-col :span="11">\
                                        <el-date-picker type="date" v-model="queryForm.StartDate" placeholder="开始日期" style = "width: 100%;" ></el-date-picker>\
                                    </el-col>\
                                    <el-col class="line" :span="2">-</el-col>\
                                    <el-col :span="11">\
                                        <el-date-picker type="date" v-model="queryForm.EndDate" placeholder="结束日期"  style="width: 100%;"></el-date-picker>\
                                    </el-col>\
                                </el-form-item>\
                            </el-col>\
                            <el-col :span="4">\
                                <el-form-item>\
                                    <el-button type="primary" @click="onSubmit" >查询</el-button>\
                                </el-form-item>\
                            </el-col>\
                        </el-row>\
                    </el-form>\
                </el-header>\
                <el-main style="height:100%;width:100%;">\
                 <el-table :data="tableData" max-height="100%" height="100%"  border >\
                   <el-table-column type="index" width="50" ></el-table-column>\
                   <el-table-column prop="Name" label="产品名" ></el-table-column>\
                   <el-table-column  label="图片" ><template slot-scope="scope"><img style="max-height:100px;max-width:100px;" :src="scope.row.ImgPath"></template></el-table-column>\
                   <el-table-column prop="RankTime" label="排名日期"  ></el-table-column>\
                   <el-table-column prop="RankLevel" label="排名"></el-table-column>\
                 </el-table>\
                </el-main>\
                <el-footer style="height:50px;">\
                 <el-row align="middle" type="flex" style="height:100%;">\
                  <el-col :span="24">\
                    <el-pagination @current-change="loadPage" layout="total,prev, pager, next,jumper" :total="total" >\
                    </el-pagination>\
                  </el-col>\
                 </el-row>\
                </el-footer>\
              </el-container>'
    ,
    methods: {
        createView: function () {
            this.$root.navselected = this.$route.query.index;
            this.loadPage(1);
        },
        loadData: function (start) {   
            googleBrower.GetCatalogProductRankList(JSON.stringify({
                CatalogID: this.$route.query.id,
                Name: this.queryForm.Name,
                RankNumber: this.queryForm.RankNumber,
                StartDate: this.dateFormat(this.queryForm.StartDate),
                EndDate: this.dateFormat(this.queryForm.EndDate, 23, 59, 59)
            }), start, this.limit, function (json) {
                var rs = JSON.parse(json);     
                if (rs.Success) {
                    this.tableData = rs.Data;
                    this.total = rs.Total;
                } 
            }.bind(this));
        },
        loadPage: function (page) { 
            this.loadData((page-1) * this.limit)
        },
        onSubmit: function () {
            this.loadPage(1);
        },
        dateFormat: function (date, hours, min, second,ms) {
            return new Date(date.getFullYear(), date.getMonth(),date.getDate(),hours||0,min||0,second||0,ms||0);
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