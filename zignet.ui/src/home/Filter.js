import React, { Component } from 'react';

class Filter extends Component {
	constructor(props) {
		super(props)
		this.state = {
			showLoopNet: false,
			showLmMobile: false,
		};

		this.handleInputChange = this.handleInputChange.bind(this);
	}

	handleInputChange(event) {
		const target = event.target;
		const value = target.checked;
		const name = target.name;

		this.setState({
			[name]: value
		})
	}


  render() {
    return (
      <div>
				<div className="form-check">
				  <input 
				  	name="showLoopNet"
				  	type="checkbox" 
				  	checked={this.state.showLoopNet}
				  	onChange={this.handleInputChange}
				  	className="form-check-input"
				  	id="showLoopNet"/>
				  <label className="form-check-label" htmlFor="showLoopNet">LoopNet</label>
				</div>
				<div className="form-check">
				  <input 
				  	name="showLmMobile"
				  	type="checkbox"
				  	checked={this.state.showLmMobile}
				  	onChange={this.handleInputChange}
				  	className="form-check-input"
				  	id="showLmMobile"/>
				  <label className="form-check-label" htmlFor="showLmMobile">LmMobile</label>
				</div>
      </div>
    );
  }
}

export default Filter;
