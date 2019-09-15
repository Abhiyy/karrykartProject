'use strict';
import styles from './'; 
const e = React.createElement;

class BasicDetails extends React.Component{
    render(){
        return(
            <div className="row">
            <div className="col-lg-12">
            <div className="col-lg-6">OrderID: </div>
            <div className="col-lg-6">Value </div>
            <div className="col-lg-6">Placed On: </div>
            <div className="col-lg-6">Value </div>
            <div className="col-lg-6">Status </div>
            <div className="col-lg-6">Dropdown </div>
            </div>
            </div>
            )
    }
}

class OrderContainer extends React.Component {
    constructor() {
      
    }

    render() {
        return(
            <BasicDetails />
            )
    }
}

const domContainer = document.querySelector('#orderDetails');
ReactDOM.render(e(OrderContainer), domContainer);